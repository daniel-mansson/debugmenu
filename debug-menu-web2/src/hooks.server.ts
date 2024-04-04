import { JWT_SECRET } from "$env/static/private";
import { lucia } from "$lib/server/auth";
import type { Handle } from "@sveltejs/kit";
import pkg from 'jsonwebtoken';
const { sign } = pkg;
import { type SignOptions } from "jsonwebtoken";

export const handle: Handle = async ({ event, resolve }) => {
	const sessionId = event.cookies.get(lucia.sessionCookieName);
	if (!sessionId) {
		event.locals.user = null;
		event.locals.session = null;
		return resolve(event);
	}

	const { session, user } = await lucia.validateSession(sessionId);
	if (session && session.fresh) {
		const sessionCookie = lucia.createSessionCookie(session.id);
		// sveltekit types deviates from the de-facto standard
		// you can use 'as any' too
		event.cookies.set(sessionCookie.name, sessionCookie.value, {
			path: ".",
			...sessionCookie.attributes
		});
	}
	if (!session) {
		const sessionCookie = lucia.createBlankSessionCookie();
		event.cookies.set(sessionCookie.name, sessionCookie.value, {
			path: ".",
			...sessionCookie.attributes
		});
	}
	event.locals.user = user;
	event.locals.session = session;

	if (session) {
		var signOptions: SignOptions = {
			issuer: "debugmenu.io",
			subject: user?.id,
			audience: "https://debugmenu.io",
			algorithm: "HS256",
			expiresIn: "7d",
			keyid: "dm",

		};
		event.locals.jwt = sign(session, JWT_SECRET, signOptions);
	}
	return resolve(event);
};