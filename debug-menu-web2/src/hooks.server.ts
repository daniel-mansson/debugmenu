import { SvelteKitAuth } from "@auth/sveltekit"
import { GOOGLE_ID, GOOGLE_SECRET, JWT_SECRET } from "$env/static/private"
import pkg from "pg";
const { Pool } = pkg;
import PostgresAdapter from "@auth/pg-adapter"
import Google from "@auth/core/providers/google"
import jwt from "jsonwebtoken";
const { sign, verify } = jwt

const pool = new Pool({
    host: "localhost",
    database: "debugmenu2",
    user: "postgres",
    password: "postgres",
    max: 20,
    idleTimeoutMillis: 30000,
    connectionTimeoutMillis: 2000
});

export const handle = SvelteKitAuth({
    adapter: PostgresAdapter(pool),
    providers: [
        Google({
            id: "google",
            name: "Google",
            clientId: GOOGLE_ID,
            clientSecret: GOOGLE_SECRET,
            authorization: {
                params: {
                    prompt: "consent",
                    access_type: "offline",
                    response_type: "code"
                }
            },
            profile(profile) {
                console.log('================= p ===================')
                console.log(JSON.stringify(profile))
                console.log(JSON.stringify(profile.id))
                return {
                    id: profile.sub,
                    name: profile.name,
                    email: profile.email,
                    image: profile.picture,
                };
            },
        })
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

            let buf = JWT_SECRET;
            let jwt = sign(session, buf, signOptions);
            session.jwt = jwt;

            return session;
        }
    }
})
