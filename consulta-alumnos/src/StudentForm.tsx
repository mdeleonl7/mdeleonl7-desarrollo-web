import { useState, useEffect } from "react";

interface Student {
    Carnet: string;
    Estudiante: string;
    Email: string;
    Seccion: string;
}

function StudentForm () {
    const [carnet, setCarnet] = useState("");
    const [students, setStudents] = useState<Student[]>([]);
    const [filteredStudent, setFilteredStudent] = useState<Student | null>(
        null
    );

    useEffect(() => {
        // Cargar toda la data cuando el componente se monta
        fetch("https://test-deploy-12.onrender.com/estudiantes/")
            .then((response) => response.json())
            .then((data) => {
                setStudents(data);
            })
            .catch((err) => console.log(err.message));
    }, [carnet]);

    const handleSearch = () => {
        // Buscar el estudiante en la data cargada
        const student = students.find((s) => s.Carnet === carnet);
        setFilteredStudent(student || null);
    };

    const handleReset = () => {
        setCarnet("");
        setFilteredStudent(null);
    };

    return (
        <div className="container">
            <div className="row justify-content-center align-items-center min-vh-100">
                <div className="col-5 mx-auto border border-white p-3">
                    <h1>Consulta de alumnos</h1>
                    <div>
                        <label className="form-label">Carnet:</label>
                        <input
                            type="text"
                            className="form-control"
                            value={carnet}
                            onChange={(e) => setCarnet(e.target.value)}
                        />
                    </div>
                    {filteredStudent && (
                        <div>
                            <label className="form-label">Nombres: </label>
                            <input
                                type="text"
                                className="form-control"
                                value={filteredStudent.Estudiante}
                                readOnly
                            />
                            <label className="form-label">
                                Correo Electrónico:{" "}
                            </label>
                            <input
                                type="text"
                                className="form-control"
                                value={filteredStudent.Email}
                                readOnly
                            />
                            <label className="form-label">Sección: </label>
                            <input
                                type="text"
                                className="form-control"
                                value={filteredStudent.Seccion}
                                readOnly
                            />
                        </div>
                    )}
                    <div>
                        <button
                            onClick={handleSearch}
                            className="btn btn-info me-2 my-2"
                        >
                            Buscar
                        </button>
                        <button
                            onClick={handleReset}
                            className="btn btn-danger me-2 my-2"
                        >
                            Limpiar
                        </button>
                        <button
                            onClick={() => console.log("Cancelar")}
                            className="btn btn-warning me-2 my-2"
                        >
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default StudentForm;
