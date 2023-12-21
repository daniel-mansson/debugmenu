import { DebugMenuBackend } from '$lib/backend/backend.js';


export async function load({ params, locals }) {
    let session = await locals.getSession();

    let instancesResponse = await DebugMenuBackend(fetch, session.jwt).getInstancesByToken(
        Number(params.tokenId)
    );

    return {
        instances: await instancesResponse.json(),
    };
}