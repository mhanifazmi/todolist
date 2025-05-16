import React, { useEffect, useState } from 'react';
import axios from 'axios';
import environment from '../../configs/environment.js';

export default function Edit({ todo, onDone }) {
    const [title, setTitle] = useState('');
    const [date, setDate] = useState('');

    useEffect(() => {
        if (todo) {
            setTitle(todo.Title);
            setDate(todo.Date?.split('T')[0] ?? '');
        }
    }, [todo]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await axios.put(environment.API_URL + `api/to-do/${todo.Id}`, {
                Title: title,
                Date: date,
            });
            onDone();
        } catch (err) {
            console.error('Edit failed', err);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div className="card">
                <div className="card-header">
                    <b>Edit Task</b>
                </div>
                <div className="card-body">
                    <div className="mb-3">
                        <label className="form-label">Title</label>
                        <input
                            className="form-control"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                        />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Date</label>
                        <input
                            type="date"
                            className="form-control"
                            value={date}
                            onChange={(e) => setDate(e.target.value)}
                        />
                    </div>
                </div>
                <div className="card-footer text-muted text-end">
                    <button
                        className="btn btn-light me-2"
                        type="button"
                        onClick={onDone}
                    >
                        Cancel
                    </button>
                    <button className="btn btn-success" type="submit">
                        Create
                    </button>
                </div>
            </div>
        </form>
    );
}
