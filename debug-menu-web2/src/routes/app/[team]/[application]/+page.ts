
/** @type {import('./$types').PageLoad} */
export function load({ params, fetch }) {
    return {
        fetch,
        applicationId: params.application
    }
}