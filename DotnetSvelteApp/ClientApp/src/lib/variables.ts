import { writable, type Writable } from "svelte/store";
import { browser } from "$app/env";
export const variables = {
    baseUrl: import.meta.env.ASPNETCORE_URLS,
};

export class User {
    token: string;
    #expiration: Date;
    userName: string;
    email: string;
    get isAuthenticated() {
        return !!this.token;
    };

    set expiration(date: Date | string) {
        this.#expiration = typeof date === "string" ? new Date(date) : date;
    }
    get expiration() {
        return this.#expiration;
    }

    constructor(u?: { token: string; expiration: string | Date; userName: string; email: string }) {
        if (u) {
            this.token = u.token;
            this.userName = u.userName;
            this.email = u.email;
            this.expiration = u.expiration;
            if (this.expiration <= new Date()) {
                this.token = "";
                if (browser) {
                    localStorage.removeItem("user");
                }
            }
        }
    }
}
export const user: Writable<User> = writable(new User());

if (browser) {
    const userString = localStorage.getItem("user");
    if (userString) {
        user.set(new User(JSON.parse(userString)));
    }
}
user.subscribe(u => {
    if (browser) {
        localStorage.setItem("user", JSON.stringify(u));
    }
});