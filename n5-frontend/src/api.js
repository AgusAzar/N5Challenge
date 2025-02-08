import axios from "axios"

const api = axios.create({
    baseURL: "http://localhost:5086/api"
});

export const getPermissionTypes = () => api.get("/PermissionTypes")

export const getPermissions = () => api.get("/permissions")
export const requestPermission = (permission) => api.post("/permissions",permission)
export const modifyPermission = (id, permission) => api.put(`/permissions/${id}`,permission)