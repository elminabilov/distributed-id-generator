# Distributed Snowflake-Style ID Generator (C#)

This repository contains a **high-throughput, horizontally scalable, Snowflake-inspired
distributed ID generator** implemented in **C#**, designed with **no single point of failure**.

The solution validates **global uniqueness under concurrent load** using a
multi-pod Docker setup and provides **post-generation and post-import verification**
via distinct checks.

---

## 📑 Table of Contents

- [Problem Statement](#problem-statement)
- [Solution Overview](#solution-overview)
- [ID Structure](#id-structure)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [License](#license)
---

## 🧩 Problem Statement

In distributed systems, generating unique identifiers at scale introduces several
challenges:

- Avoiding centralized coordination
- Maintaining high throughput
- Ensuring global uniqueness across nodes
- Preventing single points of failure
- Supporting horizontal scalability

Traditional database-backed or sequential ID strategies often become bottlenecks
or availability risks.

---

## 🛠️ Solution Overview

This project implements a **Snowflake-style ID generation algorithm** in C#.
Each instance (pod):

- Generates IDs independently
- Uses a unique **machine identifier**
- Produces time-sortable, globally unique IDs
- Does not rely on shared state or databases

To validate correctness under real conditions, IDs are generated concurrently
across multiple pods and verified for uniqueness.

---

## 🆔 ID Structure

Each generated ID is composed of:

- **Timestamp** – ensures time ordering
- **Datacenter ID** – logical grouping
- **Machine ID** – uniqueness per pod
- **Sequence number** – handles high-frequency generation within the same millisecond

This structure allows deterministic uniqueness without coordination.

---

## Architecture

---

## 🚀 Getting Started

### Prerequisites

- Docker
- .NET 10 (or later) SDK installed. Any IDE(Visual Studio, Visual Studio Code, Rider)

### Local Setup

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/elminabilov/distributed-id-generator.git
   cd src
   ```

2. **Run the docker compose file:**
   ```bash
   docker compose -f docker-compose.yaml -p src up -d
   ```

3. **Run the following postgre command in the shell of first pod to ensure id count equals distinct id count:**
   - make check file executable
   ```bash
   chmod +x ../tests/docker_id_duplication_check.sh
   ```
   - execute file to see results
   ```bash
   ../tests/docker_id_duplication_check.sh 
   ```
4. **Compare result of total_rows count vs distinct_rows**
   <img width="1150" height="351" alt="Screenshot from 2026-01-30 09-00-39" src="https://github.com/user-attachments/assets/ee7ae10e-b7a6-4501-b6ac-6e36a10346dd" />

---

## 📝 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.