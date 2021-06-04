# capacitor-udp-requests

The plugin is able to send UDP requests to an address and port with a specified payload

## Install

```bash
npm install capacitor-udp-requests
npx cap sync
```

## API

<docgen-index>

* [`echo(...)`](#echo)
* [`sendUdpRequest(...)`](#sendudprequest)
* [Interfaces](#interfaces)

</docgen-index>

<docgen-api>
<!--Update the source file JSDoc comments and rerun docgen to update the docs below-->

### echo(...)

```typescript
echo(options: { value: string; }) => any
```

| Param         | Type                            |
| ------------- | ------------------------------- |
| **`options`** | <code>{ value: string; }</code> |

**Returns:** <code>any</code>

--------------------


### sendUdpRequest(...)

```typescript
sendUdpRequest(options: SendUdpRequestOptions) => any
```

| Param         | Type                                                                    |
| ------------- | ----------------------------------------------------------------------- |
| **`options`** | <code><a href="#sendudprequestoptions">SendUdpRequestOptions</a></code> |

**Returns:** <code>any</code>

--------------------


### Interfaces


#### SendUdpRequestOptions

| Prop          | Type                |
| ------------- | ------------------- |
| **`port`**    | <code>string</code> |
| **`address`** | <code>string</code> |
| **`payload`** | <code>string</code> |

</docgen-api>
