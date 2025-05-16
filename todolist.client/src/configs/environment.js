const environment = {
    API_URL: import.meta.env.VITE_API_URL || 'http://localhost:7238',
    TIMEZONE: import.meta.env.VITE_TIMEZONE || 'Asia/Kuala_Lumpur',
};

export default environment;