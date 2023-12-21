import { SvelteKitAuth } from "@auth/sveltekit"
import Google from "@auth/core/providers/google"
import { GOOGLE_ID, GOOGLE_SECRET, JWT_SECRET } from "$env/static/private"
import { sign, verify, type SignOptions } from "jsonwebtoken";
import PostgresAdapter from "@auth/pg-adapter"
import { Pool } from "pg"

const pool = new Pool({
    host: "localhost",
    database: "debugmenu",
    user: "postgres",
    password: "knowingly",
    max: 20,
    idleTimeoutMillis: 30000,
    connectionTimeoutMillis: 2000
});

export const handle = SvelteKitAuth({
    adapter: PostgresAdapter(pool),
    providers: [
        Google({ clientId: GOOGLE_ID, clientSecret: GOOGLE_SECRET })
    ],
    session: {
        strategy: "jwt",
        maxAge: 60 * 60 * 24 * 30,
    },
    callbacks: {
        jwt: async (obj) => {
            return obj.token
        },
        session: async ({ session, token, user }) => {

            // console.log('');
            // console.log(`SESSION\nsession: ${JSON.stringify(session)} \nuser: ${JSON.stringify(user)} \ntoken: ${JSON.stringify(token)}`);

            var signOptions: SignOptions = {
                issuer: "debugmenu.io",
                subject: `${token?.email}`,
                audience: "http://debugmenu.io",
                algorithm: "HS256",
                expiresIn: "7d",
            };

            console.log(JSON.stringify(session))
            session.userId = token.sub;
            session.jti = token.sub;

            // let buf = Buffer.from(JWT_SECRET, 'base64');
            let buf = JWT_SECRET;
            //console.log(buf)
            let jwt = sign(session, buf, signOptions);
            // user.backendJwt = jwt;

            session.jwt = jwt;

            let asdf = verify(jwt, buf);

            //console.log(JSON.stringify(asdf))

            return session;
        }
    }
})
