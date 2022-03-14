import { get } from 'svelte/store';
import type { Forecast } from './forecast';
import { User, user } from './variables';
interface UserResponse {
    token: string;
    expiration: string;
    userName: string;
    email: string;
    errors?: { code: string, description: string }[]
}
// use fetch to make a request to the server to get the token and expiration date
export const login = async (email: string, password: string) => {
    const result = await fetch("/api/authenticate/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            email,
            password,
        }),
    });
    const res: UserResponse = await result.json();
    if (result.ok) {
        user.update(u => {
            u = new User(res);
            return u;
        });
    } else if (result.status === 401) {
        return [{ code: 'Unauthorized', description: 'Invalid Username/Password combination.' }]
    }
    return res.errors;
};

export const register = async (userName: string, email: string, password: string) => {
    const result = await fetch("/api/authenticate/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            userName,
            email,
            password,
        }),
    });
    if (result.ok) {
        const res: User = await result.json();
        user.update(u => {
            u = new User(res);
            return u;
        });
    }
    if (result.status === 401) {
        return { Error: ['Invalid email and password combination'] };
    }
    return (await result.json()).errors;
};
async function sendRequest<T>(url: string, method: string, body?: unknown): Promise<T> {
    const localUser = get(user);
    const result = await fetch(url, {
        method,
        body: body ? JSON.stringify(body) : undefined,
        headers: {
            "Authorization": `Bearer ${localUser.token}`,
        }
    });
    if (result.ok) {
        return result.json();
    }
    else {
        throw new Error(result.statusText);
    }
}

export const getForecast = () => sendRequest<Forecast[]>('/api/forecast', 'GET');

