
# Credentials Store Service

### Student project for Cyber Security (Osnove informacione bezbednosti)

### TEAM:
#### Vladislav Petković (Dis)
#### Nikola Tomašević
#### Luka Šarić
#### Dušan Tomić
## 


Develop a secure **Credentials Store (CS)** for managing user accounts with **hashed passwords** and **account policies** (e.g., failed login limits, inactivity rules).  

Key components:  
- **IAccount Management** (Admins only): Create, delete, lock, enable, and disable accounts while enforcing security policies.  
- **AuthenticationService (AS)**: Handles user login/logout, verifies credentials via CS, and tracks logged-in users.  

Security:  
- **Mutual authentication** between CS and AS via **certificates**.  
- **AES-CBC encryption** and **digital signatures** for communication.  
- **Windows Authentication Protocol** for client interactions.  

User roles:  
- **Account Users**: Can log in/logout via AS but cannot manage accounts.  
- **Account Admins**: Manage accounts via CS but cannot use AS login services.


