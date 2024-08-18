<template>
  <div class="container mt-4">
    <h2>Socket</h2>
    <table class="table">
      <thead>
      <tr>
        <th>Number</th>
        <th>Message</th>
        <th>Time</th>
      </tr>
      </thead>
      <tbody id="messages">
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      socket: null
    };
  },
  mounted() {
    const baseUrl = import.meta.env.VITE_SERVER_URL;
    this.socket = new WebSocket(`${baseUrl.replace('http', 'ws')}/ws`);

    this.socket.onmessage = (event) => {
      const messageData = JSON.parse(event.data);
      const messagesTable = document.getElementById('messages');
      const row = document.createElement('tr');

      const numberCell = document.createElement('td');
      const messageCell = document.createElement('td');
      const timeCell = document.createElement('td');

      numberCell.textContent = messageData.SerialNumber;
      messageCell.textContent = messageData.Message;
      timeCell.textContent = this.formatDateToLocal(messageData.Date);

      row.appendChild(numberCell);
      row.appendChild(messageCell);
      row.appendChild(timeCell);

      messagesTable.appendChild(row);
    };

    this.socket.onopen = () => {
      console.log('WebSocket. Connection established');
    };

    this.socket.onclose = () => {
      console.log('WebSocket. Connection closed');
    };

    this.socket.onerror = (error) => {
      console.error('WebSocket. Error:', error);
    };
  },
  beforeUnmount() {
    if (this.socket) {
      this.socket.close();
      console.log('WebSocket connection closed');
    }
  },
  methods: {
    formatDateToLocal(dateStr) {
      const date = new Date(dateStr);
      return `${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    }
  }
};
</script>

<style scoped>
.table {
  border-collapse: collapse;
  width: 100%;
}

.table th, .table td {
  padding: 8px;
  text-align: left;
}

.table th {
  background-color: #f8f9fa;
}

.table td {
  border-bottom: 1px solid #dee2e6;
}
</style>
