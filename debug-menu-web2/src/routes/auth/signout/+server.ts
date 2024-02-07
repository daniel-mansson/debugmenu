import { google, lucia } from "$lib/server/auth";
import { OAuth2RequestError } from "arctic";
import { generateId } from "lucia";
import { db } from "$lib/server/db";

import { redirect, type RequestEvent } from "@sveltejs/kit";
import type { DatabaseUser } from "$lib/server/db";

export async function GET(event: RequestEvent): Promise<Response> {
	if (event.locals.session) {
		lucia.invalidateSession(event.locals.session.id);
		const sessionCookie = lucia.createBlankSessionCookie();
		event.cookies.set(sessionCookie.name, sessionCookie.value, {
			path: ".",
			...sessionCookie.attributes
		});
	}

	return redirect(302, '/auth/signin');
}

interface GoogleUser {
	id: string;
	login: string;
}
