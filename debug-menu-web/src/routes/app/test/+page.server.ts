export const ssr = false;

export async function load({ params, locals }) {
    let session = await locals.getSession();

    return {
        session: session
    };
}