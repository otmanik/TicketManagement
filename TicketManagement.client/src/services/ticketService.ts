import { Ticket } from "../types/Ticket";
import { TicketFormData } from "../components/TicketForm";
import { API_BASE_URL } from "../constants";

export interface TicketResponse {
  tickets: Ticket[];
  pageNumber: number;
  totalCount: number;
  totalPages: number;
}

export const fetchTickets = async (
  page: number,
  rowsPerPage: number
): Promise<TicketResponse> => {
  const response = await fetch(
    `${API_BASE_URL}/tickets?pageNumber=${page}&pageSize=${rowsPerPage}`
  );
  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }
  return await response.json();
};

export const createTicket = async (ticket: TicketFormData): Promise<Ticket> => {
  const response = await fetch(`${API_BASE_URL}/tickets`, {
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
  return await response.json();
};

export const updateTicket = async (
  id: number,
  ticket: TicketFormData
): Promise<Ticket> => {
  const response = await fetch(`${API_BASE_URL}/tickets/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      ...ticket,
      id,
    }),
  });
  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }
  return await response.json();
};

export const deleteTicket = async (id: number): Promise<void> => {
  const response = await fetch(`${API_BASE_URL}/tickets/${id}`, {
    method: "DELETE",
  });
  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }
};
