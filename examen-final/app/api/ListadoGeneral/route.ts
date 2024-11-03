import { conn } from "@/app/utils/db";
import { NextRequest } from "next/server";

export async function GET(req: NextRequest) {
	try {
		const query =
			"SELECT * FROM tbl_infocliente INNER JOIN tbl_cliente ON tbl_cliente.id = tbl_infoclinete.id_cliente ORDER BY tbl_infocliente.fecha_creacion, tbl_cliente.apellido";
		const resultado = await conn.query(query, []);
		return Response.json(resultado.rows[0]);
	} catch (error) {}
}