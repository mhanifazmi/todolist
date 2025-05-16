import environment from '../configs/environment.js';

export function convertToLocal(dateString, timeZone = environment.TIMEZONE) {
    const date = new Date(dateString);

    return date.toLocaleString('en-MY', {
        timeZone,
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        hour12: true,
    });
}

export function convertToLocalDateOnly(
    dateString,
    timeZone = environment.TIMEZONE
) {
    const date = new Date(dateString);

    return date.toLocaleDateString('en-MY', {
        timeZone,
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
    });
}
