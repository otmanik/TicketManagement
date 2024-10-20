import React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  Typography,
  TablePagination,
} from "@material-ui/core";
import { Ticket } from "../types/Ticket";

interface TicketResponse {
  tickets: Ticket[];
  pageNumber: number;
  pageSize: number;
}

interface TicketTableProps {
  ticketData: TicketResponse | null;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onPageChange: (newPage: number) => void;
  onRowsPerPageChange: (newRowsPerPage: number) => void;
}

const TicketTable: React.FC<TicketTableProps> = ({
  ticketData,
  onEdit,
  onDelete,
  onPageChange,
  onRowsPerPageChange,
}) => {
  if (!ticketData) {
    return <Typography>Loading tickets...</Typography>;
  }

  const { tickets, pageNumber, pageSize } = ticketData;

  const handleChangePage = (event: unknown, newPage: number) => {
    onPageChange(newPage + 1);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    onRowsPerPageChange(parseInt(event.target.value, 10));
  };

  return (
    <Paper>
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Date</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {tickets.length === 0 ? (
              <TableRow>
                <TableCell colSpan={5} align="center">
                  No tickets found
                </TableCell>
              </TableRow>
            ) : (
              tickets.map((ticket) => (
                <TableRow key={ticket.id}>
                  <TableCell>{ticket.id}</TableCell>
                  <TableCell>{ticket.description}</TableCell>
                  <TableCell>{ticket.status}</TableCell>
                  <TableCell>
                    {new Date(ticket.date).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    <Button onClick={() => onEdit(ticket.id)}>Edit</Button>
                    <Button onClick={() => onDelete(ticket.id)}>Delete</Button>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        component="div"
        count={-1}
        page={pageNumber - 1}
        onPageChange={handleChangePage}
        rowsPerPage={pageSize}
        onRowsPerPageChange={handleChangeRowsPerPage}
        rowsPerPageOptions={[5, 10, 25]}
      />
    </Paper>
  );
};

export default TicketTable;
