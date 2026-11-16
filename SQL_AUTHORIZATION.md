# RBAC SQL procedure rollout notes

`scripts/004_authorized_data_procedures.sql` adds server-side permission-aware procedures for document list/search/file lookup/upload metadata, advertisement and publishing reads/writes, and transactional user role/status changes with last-active-admin safeguards.

Test on a restored disposable database after scripts `002_rbac_permissions.sql` and `003_user_provisioning.sql`. The current forms have not all been refactored to call these procedures, and the procedures take `@ActorUsername`. In this desktop application that value is not a trusted identity if the client can choose it. Therefore this migration is defense-in-depth and a refactoring path, not production-grade authorization by itself.

For enforcement, use a trusted service/API or per-user SQL identity mapping, grant the runtime only EXECUTE on approved procedures, and remove direct table access. Ensure stored procedures derive actor identity from the trusted execution context; do not rely solely on a client-supplied username.
