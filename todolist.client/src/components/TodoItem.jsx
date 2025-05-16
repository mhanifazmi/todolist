import React from 'react';
import { convertToLocal } from '../utils/DateTimeHelper';

export default function TodoItem({ todo, onToggleComplete, onDelete, onEdit }) {
    return (
        <div className="d-flex justify-content-between align-items-center">
            <div>
                <h5
                    className={`mb-1 ${
                        todo.IsCompleted ? 'text-decoration-line-through' : ''
                    }`}
                >
                    {todo.Title}
                </h5>
                <p className="text-muted mb-0">
                    {!todo.IsCompleted ? (
                        <span className="badge bg-warning text-dark">
                            Pending
                        </span>
                    ) : (
                        <span className="badge bg-success">
                            Completed at {convertToLocal(todo.CompletedAt)}
                        </span>
                    )}
                </p>
            </div>
            <div className="d-flex align-items-center">
                <button
                    className="btn btn-outline-success me-2"
                    onClick={() => onToggleComplete(todo.Id)}
                >
                    <i
                        className={`bi ${
                            todo.IsCompleted
                                ? 'bi-arrow-counterclockwise'
                                : 'bi-check-lg'
                        }`}
                    ></i>
                </button>
                <button
                    className="btn btn-outline-primary me-2"
                    onClick={() => onEdit(todo)}
                >
                    <i className="bi bi-pencil-square"></i>
                </button>
                <button
                    className="btn btn-outline-danger"
                    onClick={() => onDelete(todo.Id)}
                >
                    <i className="bi bi-trash"></i>
                </button>
            </div>
        </div>
    );
}
