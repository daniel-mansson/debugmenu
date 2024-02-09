
/** @type {import('./$types').PageServerLoad} */
export function load({ params, locals }) {
    return {
        user: locals.user,
        backendToken: locals.jwt,
    }
}