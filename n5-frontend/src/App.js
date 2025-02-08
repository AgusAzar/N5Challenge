import "./App.css"
import React, { useState, useEffect } from 'react';
import { getPermissions, requestPermission, modifyPermission, getPermissionTypes } from './api';
function App() {
  const [permissions, setPermissions] = useState([]);
  const [permissionTypes, setPermissionTypes] = useState([]);
  const [selectedPermission, setSelectedPermission] = useState(null);
  const [formData, setFormData] = useState({
    nombreEmpleado: '',
    apellidoEmpleado: '',
    TipoPermisoId: -1,
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const permissionTypesResponse = await getPermissionTypes();
      setPermissionTypes(permissionTypesResponse.data)
      const permissionsResponse = await getPermissions();
      setPermissions(permissionsResponse.data);
    } catch (error) {
      console.error("Error fetching permissions:", error);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSelectPermission = (permission) => {
    setSelectedPermission(permission);
    setFormData({
      nombreEmpleado: permission.nombreEmpleado,
      apellidoEmpleado: permission.apellidoEmpleado,
      tipoPermisoId: permission.tipoPermisoId,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      debugger
      if (selectedPermission) {
        await modifyPermission(selectedPermission.id, formData);
        alert("Permiso modificado exitosamente!");
      } else {
        await requestPermission(formData);
        alert("Permiso creado exitosamente!");
      }
      fetchData();
      resetForm();
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const resetForm = () => {
    setSelectedPermission(null);
    setFormData({
      nombreEmpleado: '',
      apellidoEmpleado: '',
      TipoPermiso: -1,
    });
  };

  return (
    <div className="App">
      <h1>Registro de Permisos</h1>

      <form onSubmit={handleSubmit}>
        <div>
          <label>Nombre Empleado:</label>
          <input
            type="text"
            name="nombreEmpleado"
            className="input"
            value={formData.nombreEmpleado}
            onChange={handleInputChange}
            required
          />
        </div>
        <div>
          <label>Apellido Empleado:</label>
          <input
            type="text"
            name="apellidoEmpleado"
            className="input"
            value={formData.apellidoEmpleado}
            onChange={handleInputChange}
            required
          />
        </div>
        <div>
          <label>Tipo de Permiso:</label>
          <select value={formData.tipoPermisoId} defaultValue={-1} className="input" onChange={handleInputChange} name="tipoPermisoId">
            <option value={-1} disabled> -- seleccione un permiso --</option>
            {permissionTypes.map((el) => (
              <option key={el.id} value={el.id}>{el.descripcion}</option>
            ))}
          </select>
        </div>
        <button type="submit">
          {selectedPermission ? "Modificar Permiso" : "Crear Permiso"}
        </button>
        <button type="button" onClick={resetForm}>
          Cancelar Edición
        </button>

      </form>

      <h2>Permisos</h2>
      <table>
        <thead>
          <tr>
            <th>Empleado</th>
            <th>Permiso otorgado</th>
            <th>Fecha del permiso</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {permissions.map((permission) => {
            let fechaPermiso = new Date(permission.fechaPermiso);
            return (
              <tr key={permission.id}>
                <td>{permission.apellidoEmpleado} {permission.nombreEmpleado}</td>
                <td>{permission.tipoPermiso.descripcion}</td>
                <td>{fechaPermiso.toLocaleDateString()}</td>
                <td>
                  <button onClick={() => handleSelectPermission(permission)}>
                    Editar
                  </button>
                </td>
              </tr>

            )
          })}
        </tbody>
      </table>
    </div>
  );
}

export default App;