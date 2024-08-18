<template>
  <div class="container mt-4">
    <h2>Sender</h2>
    <div class="mb-3">
      <label for="message" class="form-label">Message:</label>
      <input v-model="message" type="text" id="message" class="form-control"/>
    </div>
    <div class="mb-3">
      <label for="serialNumber" class="form-label">Serial Number:</label>
      <input v-model.number="serialNumber" type="number" id="serialNumber" class="form-control"/>
    </div>
    <button @click="sendMessage" class="btn btn-primary">Send Message</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      message: '',
      serialNumber: 0,
    };
  },
  methods: {
    async sendMessage() {
      if (!this.message || this.serialNumber == null) {
        alert('Please enter both message and serial number.');
        return;
      }

      try {
        const baseUrl = import.meta.env.VITE_SERVER_URL;
        const response = await fetch(`${baseUrl}/api/Message`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            message: this.message,
            serialNumber: this.serialNumber
          })
        });

        if (!response.ok) {
          throw new Error('Network response was not ok');
        }

        const textResponse = await response.text();

        const numberResponse = parseInt(textResponse, 10);

        if (!isNaN(numberResponse)) {
          this.serialNumber = numberResponse;
          console.log('Message sent. Updated serialNumber:', this.serialNumber);
        } else {
          console.error('Server response is not a valid number');
        }
      } catch (error) {
        console.error('Error sending message:', error);
      }
    }
  }
};
</script>

<style scoped>
</style>
