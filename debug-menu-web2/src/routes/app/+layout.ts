import { currentApplication, currentInstance, currentTeam, currentToken } from '$lib/appstate.js';

/** @type {import('./$types').LayoutLoad} */
export async function load({ params, parent }) {

    currentTeam.set(params.team);
    currentApplication.set(params.application);
    currentToken.set(params.token);
    currentInstance.set(params.instance);

    return {
        session: parent().then(p => p.session),
        team: params.team,
        application: params.application,
        token: params.token,
        instance: params.instance,
    };
}