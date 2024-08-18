<template>
  <div id="app">
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container">
        <div class="d-flex">
          <router-link to="/" class="btn btn-outline-primary me-2">Sender</router-link>
          <router-link to="/socket" class="btn btn-outline-primary me-2">Socket</router-link>
          <router-link to="/history" class="btn btn-outline-primary me-2">History</router-link>
          <a :href="swaggerUrl" target="_blank" class="btn btn-outline-primary">Swagger</a>
        </div>
      </div>
    </nav>
    <router-view/>
  </div>
</template>

<script>
export default {
  name: 'App',
  computed: {
    swaggerUrl() {
      return `${import.meta.env.VITE_SERVER_URL}/swagger/index.html`;
    }
  }
}
</script>

<style>
</style>
