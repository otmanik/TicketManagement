export interface Ticket {
  id: number;
  description: string;
  status: string;
  date: string;
}

export type TicketFormData = Omit<Ticket, "id" | "status"> & {
  status: 0 | 1;
};
