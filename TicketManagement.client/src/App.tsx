import React, { useState, useEffect } from "react";
import { Container, Typography, Button } from "@material-ui/core";
import TicketTable from "./components/TicketTable";
import TicketForm from "./components/TicketForm";
import { Ticket } from "./types/Ticket";

interface TicketResponse {
  tickets: Ticket[];
  pageNumber: number;
  pageSize: number;
}
export type TicketFormData = Omit<Ticket, "id" | "status"> & {
  status: 0 | 1;
};

const App: React.FC = () => {
  const [ticketData, setTicketData] = useState<TicketResponse | null>(null);
  const [isFormVisible, setIsFormVisible] = useState(false);
  const [editingTicket, setEditingTicket] = useState<Ticket | undefined>(
    undefined
  );
  const [page, setPage] = useState(1);
  const [rowsPerPage, setRowsPerPage] = useState(5);

  useEffect(() => {
    fetchTickets();
  }, [page, rowsPerPage]);

  const fetchTickets = async () => {
    try {
      const response = await fetch(
        `https://localhost:7288/api/tickets?pageNumber=${page}&pageSize=${rowsPerPage}`
      );
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data: TicketResponse = await response.json();
      setTicketData(data);
    } catch (error) {
      console.error("Error fetching tickets:", error);
    }
  };

  const handleCreateTicket = async (ticket: TicketFormData) => {
    try {
      const response = await fetch("https://localhost:7288/api/tickets", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          ...ticket,
          status: ticket.status === 0 ? "Open" : "Closed",
        }),
      });
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      fetchTickets();
      setIsFormVisible(false);
    } catch (error) {
      console.error("Error creating ticket:", error);
    }
  };

  const handleUpdateTicket = async (updatedTicket: TicketFormData) => {
    if (!editingTicket) return;
    console.log("Updating ticket:", updatedTicket);
    try {
      const response = await fetch(
        `https://localhost:7288/api/tickets/${editingTicket.id}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            ...updatedTicket,
            id: editingTicket.id,
          }),
        }
      );
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      fetchTickets();
      setEditingTicket(undefined);
    } catch (error) {
      console.error("Error updating ticket:", error);
    }
  };

  const handleDeleteTicket = async (id: number) => {
    try {
      const response = await fetch(`https://localhost:7288/api/tickets/${id}`, {
        method: "DELETE",
      });
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      fetchTickets();
    } catch (error) {
      console.error("Error deleting ticket:", error);
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
