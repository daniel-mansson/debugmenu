import { currentApplication, currentBackendToken, currentInstance, currentTeam, currentToken, currentUser, teams, updateApplication, updateInstance, updateTeam, updateToken, updateUser } from '$lib/appstate.js';
import { DebugMenuBackend } from '$lib/backend/backend.js';
import { get } from 'svelte/store';

/** @type {import('./$types').LayoutLoad} */
export async function load({ params, data, fetch }) {
    currentBackendToken.set(data.backendToken);

    await updateUser(data.user, fetch);
    await updateTeam(params.team ? +params.team : undefined, fetch)
    await updateApplication(params.application ? +params.application : undefined, fetch)
    await updateToken(params.token ? +params.token : undefined, fetch)
    await updateInstance(params.instance ? +params.instance : undefined, fetch)

    return {
        team: params.team,
        application: params.application,
        token: params.token,
        instance: params.instance,
        user: data.user,
        backendToken: data.backendToken
    };
}