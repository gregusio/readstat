import React, { useEffect } from 'react'
import TextField from '@mui/material/TextField'
import userService from '../services/userService';
import followingService from '../services/followingService';
import { useNavigate, useParams } from 'react-router-dom';
import Button from '@mui/material/Button';
import { Box, Modal } from '@mui/material';


interface User {
    id: string;
    username: string;
}

interface FollowingProps {
    followingId: string;
    followingUsername: string;
    followingDate: string;
}

const style = {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    width: 500,
    minHeight: 600,
    bgcolor: "background.paper",
    border: "2px solid #000",
    boxShadow: 24,
    p: 4,
};

const Following: React.FC = () => {
    const [filteredFollowing, setFilteredFollowing] = React.useState<FollowingProps[]>([]);
    const [filteredUsers, setFilteredUsers] = React.useState<User[]>([]);
    const [users, setUsers] = React.useState<User[]>([]);
    const [following, setFollowing] = React.useState<FollowingProps[]>([]);
    const userId = useParams<{ userId: string }>().userId;
    const [openModal, setOpenModal] = React.useState(false);
    const [searchValue, setSearchValue] = React.useState("");
    const navigate = useNavigate();

    const handleOpenModal = () => {
        setOpenModal(true);
        setFilteredUsers([]);
        setFilteredFollowing(following);
        setSearchValue("");
    }

    const handleCloseModal = () => {
        setFilteredUsers([]);
        setOpenModal(false);
    }

    useEffect(() => {
        const fetchFollowingUsers = async () => {
            try {
                if (userId) {
                    const response = await followingService.getFollowing(userId);
                    setFollowing(response);
                    setFilteredFollowing(response);
                } else {
                    console.error("User ID is undefined.");
                }
            } catch (error) {
                console.error("Error fetching following users:", error);
            }
        };

        const fetchUsers = async () => {
            try {
                if (userId) {
                    const response = await userService.getAllUsers(userId);
                    setUsers(response);
                }
            } catch (error) {
                console.error("Error fetching users:", error);
            }
        };

        fetchFollowingUsers();
        fetchUsers();
    }
        , [userId]);

    const handleFollowingSearch = (event: React.ChangeEvent<HTMLInputElement>) => {
        const searchValue = event.target.value;
        setSearchValue(searchValue);
        if (searchValue) {
            const filtered = following.filter(f =>
                f.followingUsername.toLowerCase().includes(searchValue.toLowerCase())
            );
            setFilteredFollowing(filtered);
        } else {
            setFilteredFollowing(following);
        }
    };

    const handleNewUserSearch = (event: React.ChangeEvent<HTMLInputElement>) => {
        const searchValue = event.target.value;
        if (searchValue) {
            const filtered = users
                .filter(u => u.username.toLowerCase().includes(searchValue.toLowerCase()))
                .slice(0, 5);
            setFilteredUsers(filtered);
        } else {
            setFilteredUsers([]);
        }
    }


    return (
        <div className="following-page">
            <div className="following-container">
                <h1>Following</h1>
                <Button variant="text" onClick={handleOpenModal}>
                    Find users
                </Button>
                <Modal
                    open={openModal}
                    onClose={handleCloseModal}
                    aria-labelledby="modal-modal-title"
                    aria-describedby="modal-modal-description"
                >
                    <Box sx={style}>
                        <TextField
                            id="outlined-select-user"
                            label="Select User"
                            onChange={handleNewUserSearch}
                            fullWidth
                        />
                        <ul>
                            {filteredUsers.map((user) => (
                                <li key={user.id}>
                                    <div className="user-card">
                                        <div className="user-info">
                                            <h3
                                                style={{ cursor: "pointer", color: "#1976d2" }}
                                                onClick={() => navigate(`/${user.id}/profile`)}
                                            >
                                                {user.username}
                                            </h3>
                                            <Button
                                                onClick={async () => {
                                                    try {
                                                        if (userId) {
                                                            await followingService.addFollowing(user.id);
                                                            const newFollowing: FollowingProps = {
                                                                followingId: user.id,
                                                                followingUsername: user.username,
                                                                followingDate: new Date().toISOString(),
                                                            };
                                                            setFollowing(prev => [...prev, newFollowing]);
                                                            setFilteredFollowing(prev => [...prev, newFollowing]);
                                                        } else {
                                                            console.error("User ID is undefined.");
                                                        }
                                                    } catch (error) {
                                                        console.error("Error adding following user:", error);
                                                    }
                                                }}
                                                className="follow-button"
                                            >
                                                Follow
                                            </Button>
                                        </div>
                                    </div>
                                </li>
                            ))}
                        </ul>

                    </Box>
                </Modal>
                <p>This page will display the list of users you are following.</p>

                <TextField
                    label="Search users"
                    variant="outlined"
                    onChange={handleFollowingSearch}
                    fullWidth
                    margin="normal"
                    value={searchValue}
                />
                <ul>
                    {filteredFollowing.map((user) => (
                        <li key={user.followingId}>
                            <div className="user-card">
                                <div className="user-info">
                                    <h3
                                        style={{ cursor: "pointer", color: "#1976d2" }}
                                        onClick={() => navigate(`/${user.followingId}/profile`)}
                                    >
                                        {user.followingUsername}
                                    </h3>
                                    <Button
                                        onClick={async () => {
                                            try {
                                                if (userId) {
                                                    await followingService.removeFollowing(user.followingId);
                                                    const newFollowing = following.filter(f => f.followingId !== user.followingId);
                                                    setFollowing(newFollowing);
                                                    setFilteredFollowing(newFollowing);
                                                } else {
                                                    console.error("User ID is undefined.");
                                                }
                                            } catch (error) {
                                                console.error("Error removing following user:", error);
                                            }
                                        }}
                                        className="unfollow-button"
                                    >
                                        Unfollow
                                    </Button>
                                </div>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    )
}

export default Following;