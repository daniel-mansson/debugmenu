import { currentApplication, currentBackendToken, currentInstance, currentTeam, currentToken, currentUser } from '$lib/appstate.js';

/** @type {import('./$types').LayoutLoad} */
export function load({ params, data }) {
    currentTeam.set(params.team);
    currentApplication.set(params.application);
    currentToken.set(params.token);
    currentInstance.set(params.instance);
    currentUser.set(data.user);
    currentBackendToken.set(data.backendToken);

    return {
        team: params.team,
        application: params.application,
        token: params.token,
        instance: params.instance,
        user: data.user,
        backendToken: data.backendToken
    };
}