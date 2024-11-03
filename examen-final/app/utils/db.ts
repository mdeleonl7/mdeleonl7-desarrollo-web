import { Pool } from "pg";

const conn = new Pool({
	host: "localhost",
	port: 5432,
	user: "mdeleon",
	password: "7uG9KXwmEWdBQfnZhBAQ",
    database: "mdeleon",
	ssl: false,
});

export { conn };
