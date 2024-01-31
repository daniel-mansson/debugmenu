/** @type {import('./$types').LayoutLoad} */
export function load({ params }) {
    return {
        application: params.application,
        token: params.token,
        instance: params.instance,
    };
}