<template>
  <main class="flex justify-center items-center">
    <div class="bg-white w-1/3 h-60 rounded-md">
      <div class="flex justify-around items-center">
        <NButton type="info">
          Create Config
        </NButton>
        <NButton type="info">
          Map CSV
        </NButton>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { NButton } from "naive-ui"
</script>

<style scoped>
main {
  background-color: #d9d9d9;
  opacity: 0.8;
  background-image: radial-gradient(#282828 0.7px, transparent 0.7px), radial-gradient(#2b2b2b 0.7px, #d9d9d9 0.7px);
  background-size: 20px 20px;
  background-position: 0 0, 10px 10px;
}
</style>