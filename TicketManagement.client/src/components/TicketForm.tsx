import React from "react";
import { useForm, Controller } from "react-hook-form";
import {
  TextField,
  Button,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  FormHelperText,
  Grid,
  Paper,
  Typography,
} from "@material-ui/core";
import { Ticket } from "../types/Ticket";

export type TicketFormData = Omit<Ticket, "id" | "status"> & {
  status: 0 | 1;
};
interface TicketFormProps {
  ticket?: Ticket;
  onSubmit: (ticket: TicketFormData) => void;
  onCancel: () => void;
}

const TicketForm: React.FC<TicketFormProps> = ({
  ticket,
  onSubmit,
  onCancel,
}) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<TicketFormData>({
    defaultValues: {
      description: ticket?.description || "",
      status: ticket?.status === "Closed" ? 1 : 0,
      date: ticket?.date
        ? new Date(ticket.date).toISOString().split("T")[0]
        : new Date().toISOString().split("T")[0],
    },
  });

  const onSubmitForm = (data: TicketFormData) => {
    onSubmit(data);
  };

  return (
    <Paper style={{ padding: "20px", marginTop: "20px", marginBottom: "20px" }}>
      <Typography variant="h6" gutterBottom>
        {ticket ? "Edit Ticket" : "Create New Ticket"}
      </Typography>
      <form onSubmit={handleSubmit(onSubmitForm)}>
        <Controller
          name="description"
          control={control}
          rules={{
            required: "Description is required",
            maxLength: {
              value: 200,
              message: "Description must not exceed 200 characters",
            },
          }}
          render={({ field }) => (
            <TextField
              {...field}
              label="Description"
              fullWidth
              margin="normal"
              error={!!errors.description}
              helperText={errors.description?.message}
            />
          )}
        />

        <FormControl fullWidth margin="normal" error={!!errors.status}>
          <InputLabel>Status</InputLabel>
          <Controller
            name="status"
            control={control}
            rules={{ required: "Status is required" }}
            render={({ field }) => (
              <Select {...field}>
                <MenuItem value={0}>Open</MenuItem>
                <MenuItem value={1}>Closed</MenuItem>
              </Select>
            )}
          />
          {errors.status && (
            <FormHelperText>{errors.status.message}</FormHelperText>
          )}
        </FormControl>

        <Controller
          name="date"
          control={control}
          rules={{
            required: "Date is required",
          }}
          render={({ field }) => (
            <TextField
              {...field}
              label="Date"
              type="date"
              fullWidth
              margin="normal"
              InputLabelProps={{
                shrink: true,
              }}
              error={!!errors.date}
              helperText={errors.date?.message}
            />
          )}
        />

        <Grid container spacing={2} style={{ marginTop: "20px" }}>
          <Grid item>
            <Button type="submit" variant="contained" color="primary">
              {ticket ? "Update Ticket" : "Create Ticket"}
            </Button>
          </Grid>
          <Grid item>
            <Button variant="contained" onClick={onCancel}>
              Cancel
            </Button>
          </Grid>
        </Grid>
      </form>
    </Paper>
  );
};

export default TicketForm;
