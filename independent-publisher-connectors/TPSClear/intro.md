# TPSClear (Independent Publisher)

TPSClear screens UK phone numbers against the Telephone Preference Service (TPS) and Corporate TPS (CTPS) registers, the UK do-not-call lists that outbound callers must check under PECR. The service is a REST API at `api.tpsclear.co.uk` with Bearer API key authentication, documented at https://tpsclear.co.uk/developers.

The connector exposes nine actions: screen up to 200 numbers in one call (one status per number, record ID in a response header), fetch a past screening record as audit evidence, read usage, and list, add to and remove from the account's own suppression and consent lists. Authentication is an API key; the `Bearer` prefix is added by a set header policy so the user pastes the bare key.

No connector on the platform screens against the UK TPS or CTPS registers today. The nearest existing connector, Do Not Call Reported Calls, reads the US Federal Trade Commission's complaint data and is unrelated.

Contact: steffen@tpsclear.co.uk
