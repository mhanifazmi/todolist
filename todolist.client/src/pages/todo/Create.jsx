import React, { useState } from 'react';
import axios from 'axios';
import environment from '../../configs/environment.js';

export default function Create({ selectedDate, onDone }) {
    const [title, setTitle] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        await axios.post(environment.API_URL + 'api/to-do', {
            title,
            date: selectedDate,
        });
        onDone();
    };

    return (
        <form onSubmit={handleSubmit}>
            <div className="card">
                <div className="card-header">
                    <b>Create Task</b>
                </div>
                <div className="card-body">
                    <div className="form-group">
                        <label>Title</label>
                        <input
                            type="text"
                            className="form-control"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            required
                        />
                    </div>

                    <div className="form-group mt-2">
                        <label>Date</label>
                        <input
                            type="date"
                            className="form-control"
                            value={selectedDate}
                            disabled
                            readOnly
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
