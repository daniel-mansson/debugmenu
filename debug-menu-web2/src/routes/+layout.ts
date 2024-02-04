
/** @type {import('./$types').LayoutLoad} */
export function load({ params, data }) {

    return {
        session: data.session,
    };
}