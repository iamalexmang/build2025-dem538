# DEM538 Baseline Repository (Bombardier Edition)

This repo mirrors the on‑stage flow for **From Zero to Event‑Driven with Dapr & Azure Service Bus**.

## 1. Build & launch baseline

```bash
docker compose build
docker compose up -d redis inventory-api billing-api shipping-api order-api
```

Now `order-api` is at **http://localhost:5000**.

### Smoke‑test

```bash
curl -X POST http://localhost:5000/orders \
     -H "Content-Type: application/json" \
     -d '{ "orderId": 1, "amount": 99.0 }'
```

## 2. Run processor with Dapr sidecar

```bash
dapr run --components-path ./components \
         --app-id order-processor --app-port 6000 \
         -- dotnet order-processor/order-processor.dll
```

## 3. Generate load with **Bombardier**

```bash
bombardier -c 50 -n 1000 \
  -m POST \
  -b '{\"orderId\":42,\"amount\":99.0}' \
  http://localhost:5000/orders
```

Install on macOS:

```bash
brew install bombardier
```

## 4. Hot‑swap to Azure Service Bus

Set `SERVICEBUS_CONNECTION_STRING`, then:

```bash
kubectl apply -f components/orders-pubsub-servicebus.yaml
```

Done!
