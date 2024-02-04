import type { LayoutServerLoad } from "./$types"

export const load: LayoutServerLoad = async (event) => {
    let s = await event.locals.getSession();


    console.log('server auth ' + JSON.stringify(s))
    return {
        session: s,
    }
}