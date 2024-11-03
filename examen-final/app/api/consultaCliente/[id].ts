import { NextRequest, NextResponse } from "next/server";
import { conn } from "@/app/utils/db";

export async function GET(req: NextRequest, {params}:{params:{id:number}}){
	const _Id = params.id;
	try {
		const query =
			"SELECT nombre, apellido, genero, nacimiento FROM tbl_cliente WHERE id = $1";
		const resultado = await conn.query(query, [_Id]);
		return NextResponse.json(resultado.rows[0]);
	} catch (error) {}
}
