import { DebugMenuBackend } from '$lib/backend/backend.js';


export async function load({ params, locals }) {
    let session = await locals.getSession();

    if (session) {
        let applicationsResponse = await DebugMenuBackend(fetch, session.jwt).getApplicationsByUser(
            session.userId
        );

        return {
            applications: await applicationsResponse.json(),
        };
    } else {
        return {
            applications: []
        };
    }
}