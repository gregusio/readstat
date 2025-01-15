import { useState } from "react";
import { styled } from "@mui/material/styles";
import Button from "@mui/material/Button";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import CircularProgress from "@mui/material/CircularProgress";
import fileService from "../../services/fileService";

const VisuallyHiddenInput = styled("input")({
  clip: "rect(0 0 0 0)",
  clipPath: "inset(50%)",
  height: 1,
  overflow: "hidden",
  position: "absolute",
  bottom: 0,
  left: 0,
  whiteSpace: "nowrap",
  width: 1,
});

export default function InputFileUpload() {
  const [loading, setLoading] = useState(false);

  const handleFileChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (files && files[0]) {
      setLoading(true);
      try {
        const response = await fileService.uploadCsv(files[0]);
        window.location.reload();
        console.log(response);
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    }
  };

  return (
    <Button
      component="label"
      role={undefined}
      variant="contained"
      tabIndex={-1}
      startIcon={loading ? <CircularProgress size={24} /> : <CloudUploadIcon />}
      disabled={loading}
    >
      {loading ? "Uploading..." : "Upload files"}
      <VisuallyHiddenInput
        type="file"
        onChange={(event) => handleFileChange(event)}
        multiple
      />
    </Button>
  );
}
