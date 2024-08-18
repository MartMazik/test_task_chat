<template>
  <div class="container mt-4">
    <h2>History</h2>
    <div class="mb-3">
      <label for="start" class="form-label">Start Date:</label>
      <input type="datetime-local" id="start" class="form-control"/>
    </div>
    <div class="mb-3">
      <label for="end" class="form-label">End Date:</label>
      <input type="datetime-local" id="end" class="form-control"/>
    </div>
    <button @click="getHistory" class="btn btn-primary">Get History</button>
    <button @click="getLastMinute" class="btn btn-secondary ms-2">Get in the Last Minute</button>

    <div v-if="noRecords" class="alert alert-warning mt-3" role="alert">
      No records found for the selected time range.
    </div>

    <table v-if="messages.length > 0" class="table mt-3">
      <thead>
      <tr>
        <th>Number</th>
        <th>Message</th>
        <th>Time</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="msg in formattedMessages" :key="msg.id">
        <td>{{ msg.serialNumber }}</td>
        <td>{{ msg.message }}</td>
        <td>{{ msg.date }}</td>
      </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      messages: [],
      noRecords: false // Флаг для отображения уведомления
    };
  },
  computed: {
    formattedMessages() {
      return this.messages.map(msg => {
        return {
          ...msg,
          date: this.formatDateToLocal(msg.date)
        };
      });
    }
  },
  methods: {
    async getHistory() {
      const startInput = document.getElementById('start').value;
      const endInput = document.getElementById('end').value;

      const start = new Date(startInput);
      const end = new Date(endInput);

      if (!isNaN(start.getTime()) && !isNaN(end.getTime())) {
        try {
          const baseUrl = import.meta.env.VITE_SERVER_URL;
          const response = await fetch(`${baseUrl}/api/Message?startDate=${start.toISOString()}&endDate=${end.toISOString()}`);

          if (response.status === 204) {
            this.noRecords = true;
            this.messages = [];
            setTimeout(() => {
              this.noRecords = false;
            }, 3000);
          } else if (response.ok) {
            const data = await response.json();
            this.messages = data;
          } else {
            throw new Error('Network response was not ok');
          }
        } catch (error) {
          console.error('Error fetching history:', error);
        }
      } else {
        console.error('Invalid date values:', {startInput, endInput});
      }
    },
    async getLastMinute() {
      const now = new Date();
      const end = new Date(now.getTime() - (now.getTimezoneOffset() * 60000)).toISOString().slice(0, 19);
      const start = new Date(now.getTime() - 60 * 1000 - (now.getTimezoneOffset() * 60000)).toISOString().slice(0, 19);

      document.getElementById('start').value = start;
      document.getElementById('end').value = end;

      await this.getHistory();
    },
    formatDateToLocal(dateStr) {
      const date = new Date(dateStr);
      return `${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    },
    setDefaultDates() {
      const now = new Date();
      const end = new Date(now.getTime() - (now.getTimezoneOffset() * 60000)).toISOString().slice(0, 19);
      const start = new Date(now.getTime() - 10 * 60 * 1000 - (now.getTimezoneOffset() * 60000)).toISOString().slice(0, 19);

      document.getElementById('start').value = start;
      document.getElementById('end').value = end;
    }
  },
  mounted() {
    this.setDefaultDates();
  }
};
</script>

<style scoped>
.table {
  border-collapse: collapse;
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

.alert {
  margin-top: 10px;
}
</style>
