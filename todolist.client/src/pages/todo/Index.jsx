import React, { useEffect, useState } from 'react';
import axios from 'axios';
import TodoItem from '../../components/TodoItem.jsx';
import CreateForm from './Create.jsx';
import EditForm from './Edit.jsx';
import environment from '../../configs/environment.js';
import {
    DragDropContext,
    Droppable,
    Draggable
} from 'react-beautiful-dnd';

const TodoPage = () => {
    const [todos, setTodos] = useState([]);
    const [date, setDate] = useState(new Date().toISOString().split('T')[0]);
    const [mode, setMode] = useState(null);
    const [selectedTodo, setSelectedTodo] = useState(null);
    const [showCopyModal, setShowCopyModal] = useState(false);
    const [copyDate, setCopyDate] = useState('');
    const [copyTasks, setCopyTasks] = useState([]);

    const fetchTodos = async () => {
        try {
            const res = await axios.get(environment.API_URL + 'api/to-do', {
                params: { date },
            });
            setTodos(res.data.Data);
        } catch (err) {
            console.error('Fetch error:', err);
        }
    };

    const fetchCopyTasks = async (selectedDate) => {
        try {
            const res = await axios.get(`${environment.API_URL}api/to-do`, {
                params: { date: selectedDate },
            });
            setCopyTasks(res.data.Data);
        } catch (err) {
            console.error('Fetch error:', err);
        }
    };

    const handleCopyConfirm = async () => {
        try {
            await axios.post(`${environment.API_URL}api/to-do/copy`, {
                FromDate: copyDate,
                ToDate: date,
            });
            setShowCopyModal(false);
            fetchTodos();
        } catch (err) {
            console.error('Copy error:', err);
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm('Are you sure you want to delete this task?')) return;

        try {
            await axios.delete(`${environment.API_URL}api/to-do/${id}`);
            setTodos((prev) => prev.filter((t) => t.Id !== id));
        } catch (err) {
            console.error('Failed to delete task:', err);
        }
    };

    useEffect(() => {
        fetchTodos();
    }, [date]);

    const handleDone = () => {
        setMode(null);
        setSelectedTodo(null);
        fetchTodos();
    };

    const handleToggleComplete = async (id) => {
        const todo = todos.find((t) => t.Id === id);
        if (!todo) return;

        try {
            await axios.patch(environment.API_URL + `api/to-do/${id}/status`, {
                isCompleted: !todo.IsCompleted,
                completedAt: !todo.IsCompleted
                    ? new Date().toISOString()
                    : null,
            });

            setTodos((prev) =>
                prev.map((t) =>
                    t.Id === id
                        ? {
                            ...t,
                            IsCompleted: !t.IsCompleted,
                            CompletedAt: !t.IsCompleted
                                ? new Date().toISOString()
                                : null,
                        }
                        : t
                )
            );
        } catch (error) {
            console.error('Failed to toggle completion', error);
        }
    };

    const handleDragEnd = async (result) => {
        if (!result.destination) return;

        const items = Array.from(todos);
        const [movedItem] = items.splice(result.source.index, 1);
        items.splice(result.destination.index, 0, movedItem);

        setTodos(items);

        try {
            await axios.post(environment.API_URL + 'api/to-do/update-sequence', {
                ids: items.map((item) => item.Id),
            });
        } catch (err) {
            console.error('Failed to update sequence:', err);
        }
    };

    return (
        <>
            <div className="container mt-4">
                <h3>To-Do List</h3>
                <div className="row">
                    <div className="col-md-6">
                        <div className="mb-3">
                            <label>Select Date</label>
                            <input
                                type="date"
                                className="form-control"
                                value={date}
                                onChange={(e) => setDate(e.target.value)}
                            />
                        </div>

                        <div className="d-flex justify-content-between">
                            <button
                                className="btn btn-primary mt-3 mb-2"
                                onClick={() => setMode('create')}
                            >
                                Create New Task
                            </button>

                            <button
                                className="btn btn-secondary mt-3 mb-2"
                                onClick={() => setShowCopyModal(true)}
                            >
                                Copy Tasks From
                            </button>
                        </div>

                        <DragDropContext onDragEnd={handleDragEnd}>
                            <Droppable droppableId="todo-list">
                                {(provided) => (
                                    <ul
                                        className="list-group"
                                        {...provided.droppableProps}
                                        ref={provided.innerRef}
                                    >
                                        {todos.map((todo, index) => (
                                            <Draggable
                                                key={todo.Id}
                                                draggableId={todo.Id.toString()}
                                                index={index}
                                            >
                                                {(provided) => (
                                                    <li
                                                        ref={provided.innerRef}
                                                        {...provided.draggableProps}
                                                        {...provided.dragHandleProps}
                                                        className="list-group-item"
                                                    >
                                                        <TodoItem
                                                            todo={todo}
                                                            onToggleComplete={handleToggleComplete}
                                                            onDelete={handleDelete}
                                                            onEdit={() => {
                                                                setSelectedTodo(todo);
                                                                setMode('edit');
                                                            }}
                                                        />
                                                    </li>
                                                )}
                                            </Draggable>
                                        ))}
                                        {provided.placeholder}
                                    </ul>
                                )}
                            </Droppable>
                        </DragDropContext>
                    </div>

                    <div className="col-md-6">
                        {mode === 'create' && (
                            <CreateForm onDone={handleDone} selectedDate={date} />
                        )}
                        {mode === 'edit' && selectedTodo && (
                            <EditForm todo={selectedTodo} onDone={handleDone} />
                        )}
                    </div>
                </div>
            </div>

            {showCopyModal && (
                <div className="modal d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
                    <div className="modal-dialog">
                        <div className="modal-content p-3">
                            <h5>Copy Tasks From</h5>
                            <input
                                type="date"
                                className="form-control"
                                value={copyDate}
                                onChange={(e) => {
                                    setCopyDate(e.target.value);
                                    fetchCopyTasks(e.target.value);
                                }}
                            />
                            <ul className="list-group mt-3">
                                {copyTasks.map((task) => (
                                    <li key={task.Id} className="list-group-item">
                                        {task.Title}
                                    </li>
                                ))}
                            </ul>
                            <button className="btn btn-success mt-3" onClick={handleCopyConfirm}>
                                Copy Tasks
                            </button>
                            <button
                                className="btn btn-secondary mt-2"
                                onClick={() => setShowCopyModal(false)}
                            >
                                Cancel
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </>
    );
};

export default TodoPage;
