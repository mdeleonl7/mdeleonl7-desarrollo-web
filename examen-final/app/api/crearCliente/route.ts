import { NextRequest, NextResponse } from "next/server";
import { conn } from "@/app/utils/db";

export const POST = async (request: NextRequest) => {
	try {
		const jsonBody = await request.json();
		const query =
			"INSERT INTO tbl_clientes (nombre, apellido, genero, nacimiento) VALUES ($1, $2, $3, $4);";
		const values = [
			jsonBody.nombre,
			jsonBody.apellido,
			jsonBody.genero,
			jsonBody.nacimiento
		];
		const resultado = await conn.query(query, values);
		return new NextResponse(JSON.stringify("Cliente creado con exito."), {
			status: 201,
		});
	} catch (ex) {
		return new NextResponse(JSON.stringify("Error al crear cliente."), {
			status: 500,
		});
	}
};
