import { DebugMenuBackend } from '$lib/backend/backend.js';


export async function load({ params, locals }) {
    let session = await locals.getSession();
    let applicationResponse = await DebugMenuBackend(fetch, session.jwt).getApplication(
        Number(params.id)
    );
    let tokensResponse = await DebugMenuBackend(fetch, session.jwt).getTokensByApplication(
        Number(params.id)
    );

    return {
        application: await applicationResponse.json(),
        tokens: await tokensResponse.json()
    };
}