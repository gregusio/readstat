import apiClient from "./apiClient";

const uploadCsv = async (file: File) => {
  const formData = new FormData();
  formData.append("file", file);

  const response = await apiClient.post("/File/upload-csv", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  localStorage.removeItem("userBooks");

  return response.data;
};

export default {
  uploadCsv,
};
