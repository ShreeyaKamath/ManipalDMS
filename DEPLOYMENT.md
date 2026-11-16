# Deployment and security notes

## Scope and honest readiness status

This application is a Windows Forms desktop client that connects directly to SQL Server. Hashing passwords, UI role checks, SQL grants, and NTFS ACLs cannot make a hostile or compromised desktop client a trusted authorization boundary. Anyone with database connectivity can inspect the client and issue their own SQL. For multi-user production handling of sensitive documents, put authentication, authorization, file access, and database access behind a trusted server/API; the client should not hold database credentials or be trusted to assert roles. This branch is a hardened desktop/demo baseline, **not certified production-ready**.

## Clean installation

1. Review `~vs7E75.sql` and run it only against a fresh or backed-up test database first. It is non-destructive and contains no seed users. It is not a data migration for an existing demo schema.
2. Configure `App.config` for the target SQL Server and a validated TLS certificate. Never set `TrustServerCertificate=True` in production.
3. Provision a non-admin database identity using your organization’s identity/secret-management process. Do not use `sa`, `sysadmin`, `db_owner`, or store credentials in source control.
4. Provision users using `scripts/Provision-User.ps1`; execute it from a trusted admin workstation and supply the server name and username explicitly. The password is prompted interactively, not included in command history.
5. Set the document directory ACL with `scripts/Set-DocumentStorageAcl.ps1` for the exact Windows identity that runs the client. The app does not set ACLs itself.
6. Build Release on Windows with the .NET Framework 4.7.2 Developer Pack, test with a disposable SQL instance, and verify all role workflows and failure scenarios.

## Role policy in this desktop client

- Admin: documents, advertisements, and publishing.
- Editor: documents only.
- Accountant: documents, advertisements, and publishing (financial workflows).

These checks are UX controls only in this direct-to-database architecture. They must be enforced by a trusted API or database execution model before real multi-user data is used. Do not grant the desktop identity broad write or user-management permissions as a substitute.

## Operational requirements before real deployment

- Use a server-side application/API identity and database permissions narrowly scoped to procedures; make end-user identity and role claims verifiable by that service.
- Use enterprise identity (prefer managed identity/Windows auth or an OIDC-backed service) where possible, account recovery/MFA policies, and monitored account lifecycle controls.
- Make account lockout and password reset controls part of the trusted authentication service. Desktop-side counters are not resilient against client tampering or distributed guessing.
- Configure restrictive NTFS ACLs, encrypted storage/backups, malware scanning, retention, recovery drills, and audit-log monitoring.
- Use current supported .NET runtime/framework and SQL Server versions; apply patching, code signing, vulnerability review, privacy/compliance review, and incident-response procedures.
- Remove or rotate any credentials that were ever committed; deleting a script does not erase Git history. Rotate the exposed `sa` credential and review server access.

## Verification

The GitHub file-write tooling does not run MSBuild, SQL Server, or Windows ACL tests. Before release, build the solution in Release mode on Windows, run schema/user provisioning on a disposable SQL Server, test login lockout and all role paths, and verify ACLs under the actual runtime identity. Do not represent the repository as production-ready until those checks and the trusted-server authorization architecture are complete.
