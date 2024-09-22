import { useForm, SubmitHandler } from "react-hook-form";

function Form() {
	interface iCampos {
		sNombre: string;
		nCreditos: number;
		sDescripcion: string;
	}

	const {
		register,
		formState: { errors },
		handleSubmit,
	} = useForm<iCampos>();

	const onSubmit: SubmitHandler<iCampos> = (data) => {
		console.log(data);
	};

	return (
		<form
			action="https://test-deploy-12.onrender.com/cursos"
			method="post"
			onSubmit={handleSubmit(onSubmit)}
		>
			<h2>Registro de curso</h2>
			<div>
				<label htmlFor="sNombre">
					Nombre curso:
				</label>
				<br />
				<input
					type="text"
					id="nombre"
					style={{ width: "350px" }}
					{...register("sNombre", {
						required: true,
					})}
				/>
				{errors.sNombre?.type === "required" && (
					<span>Debes ingresar un nombre.</span>
				)}
			</div>
			<div>
				<label htmlFor="nCreditos">
					Creditos:
				</label>
				<br />
				<input
					type="text"
					id="creditos"
					style={{ width: "350px" }}
					{...register("nCreditos", {
						required: true,
					})}
				/>
				{errors.nCreditos?.type === "required" && (
					<span>Debes ingresar un numero.</span>
				)}
			</div>
			<div>
				<label htmlFor="sDescripcion">
					Descripcion:
				</label>
				<br />
				<input
					type="text"
					id="descripcion"
					style={{ width: "350px" }}
					{...register("sDescripcion", {
						required: true,
					})}
				/>
				{errors.sDescripcion?.type === "required" && (
					<span>Debes ingresar una descripcion.</span>
				)}
			</div>
			<br />
			<button type="submit">Guardar</button>
			<button type="reset">Limpiar</button>
		</form>
	);
}

export default Form;
