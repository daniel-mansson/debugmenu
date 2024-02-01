import { currentApplication, currentInstance, currentToken } from '$lib/appstate.js';

/** @type {import('./$types').LayoutLoad} */
export function load({ params }) {

    currentApplication.set(params.application);
    currentToken.set(params.token);
    currentInstance.set(params.instance);

    return {
        application: params.application,
        token: params.token,
        instance: params.instance,
    };
}