import React, { useState, useEffect } from "react";
import { Container, Typography, Button } from "@material-ui/core";
import TicketTable from "./components/TicketTable";
import TicketForm from "./components/TicketForm";
import { Ticket } from "./types/Ticket";
import { TicketFormData } from "./components/TicketForm";
import {
  fetchTickets,
  createTicket,
  updateTicket,
  deleteTicket,
  TicketResponse,
} from "./services/ticketService";

const App: React.FC = () => {
  const [ticketData, setTicketData] = useState<TicketResponse | null>(null);
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [editingTicket, setEditingTicket] = useState<Ticket | undefined>(
    undefined
  );
  const [page, setPage] = useState(1);
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchTicketsData();
  }, [page, rowsPerPage]);

  const fetchTicketsData = async () => {
    try {
      const data = await fetchTickets(page, rowsPerPage);
      setTicketData(data);
      setError(null);
    } catch (error) {
      console.error("Error fetching tickets:", error);
      setError("Failed to fetch tickets. Please try again later.");
    }
  };

  const handleCreateTicket = async (ticket: TicketFormData) => {
    try {
      await createTicket(ticket);
      fetchTicketsData();
      setIsFormVisible(false);
      setError(null);
    } catch (error) {
      console.error("Error creating ticket:", error);
      setError("Failed to create ticket. Please try again.");
    }
  };

  const handleUpdateTicket = async (updatedTicket: TicketFormData) => {
    if (!editingTicket) return;
    try {
      await updateTicket(editingTicket.id, updatedTicket);
      fetchTicketsData();
      setEditingTicket(undefined);
      setError(null);
    } catch (error) {
      console.error("Error updating ticket:", error);
      setError("Failed to update ticket. Please try again.");
    }
  };

  const handleDeleteTicket = async (id: number) => {
    try {
      await deleteTicket(id);
      fetchTicketsData();
      setError(null);
    } catch (error) {
      console.error("Error deleting ticket:", error);
      setError("Failed to delete ticket. Please try again.");
    }
  };

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
  };

  const handleRowsPerPageChange = (newRowsPerPage: number) => {
    setRowsPerPage(newRowsPerPage);
    setPage(1);
  };

  return (
    <Container>
      <Typography variant="h4" component="h1" gutterBottom>
        Ticket Management System
      </Typography>
      {error && (
        <Typography color="error" gutterBottom>
          {error}
        </Typography>
      )}
      <Button
        onClick={() => {
          setIsFormVisible(true);
          setEditingTicket(undefined);
        }}
        variant="contained"
        color="primary"
        style={{ marginBottom: "20px" }}
      >
        Add New Ticket
      </Button>
      {(isFormVisible || editingTicket) && (
        <TicketForm
          key={editingTicket ? editingTicket.id : "new"}
          ticket={editingTicket}
          onSubmit={(updatedTicket) => {
            if (editingTicket) {
              handleUpdateTicket(updatedTicket);
            } else {
              handleCreateTicket(updatedTicket);
            }
            setIsFormVisible(false);
            setEditingTicket(undefined);
          }}
          onCancel={() => {
            setIsFormVisible(false);
            setEditingTicket(undefined);
          }}
        />
      )}
      <TicketTable
        ticketData={ticketData}
        onEdit={(id) => {
          const ticketToEdit = ticketData?.tickets.find(
            (ticket) => ticket.id === id
          );
          if (ticketToEdit) {
            setEditingTicket(ticketToEdit);
            setIsFormVisible(false);
          }
        }}
        onDelete={handleDeleteTicket}
        onPageChange={handlePageChange}
        onRowsPerPageChange={handleRowsPerPageChange}
      />
    </Container>
  );
};

export default App;
