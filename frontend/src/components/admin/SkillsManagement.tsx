import { useState, useEffect } from 'react';
import {
  Box,
  Button,
  TextField,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import { Delete as DeleteIcon, Edit as EditIcon } from '@mui/icons-material';
import api from '../../api/axios';

interface Skill {
  id: string;
  title: string;
}

export const SkillsManagement = () => {
  const [skills, setSkills] = useState<Skill[]>([]);
  const [open, setOpen] = useState(false);
  const [editingSkill, setEditingSkill] = useState<Skill | null>(null);
  const [skillTitle, setSkillTitle] = useState('');

  const fetchSkills = async () => {
  try {
    const response = await api.get('/skill/all');
    const normalized = response.data.map((s: any) => ({
      id: s.id.value,
      title: s.title.value
    }));
    setSkills(normalized);
  } catch (error) {
    console.error('Ошибка при загрузке скиллов:', error);
  }
};

  useEffect(() => {
    fetchSkills();
  }, []);

  const handleOpen = (skill?: Skill) => {
    if (skill) {
      setEditingSkill(skill);
      setSkillTitle(skill.title);
    } else {
      setEditingSkill(null);
      setSkillTitle('');
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingSkill(null);
    setSkillTitle('');
  };

  const handleSubmit = async () => {
    try {
      if (editingSkill) {
        await api.put(`/skills/${editingSkill.id}`, { title: skillTitle });
      } else {
        await api.post('/skill/add', { title: skillTitle });
      }
      handleClose();
      fetchSkills();
    } catch (error) {
      console.error('Ошибка при сохранении скилла:', error);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await api.delete(`/skills/${id}`);
      fetchSkills();
    } catch (error) {
      console.error('Ошибка при удалении скилла:', error);
    }
  };

  return (
    <Box>
      <Box sx={{ mb: 2 }}>
        <Button variant="contained" onClick={() => handleOpen()}>
          Добавить скилл
        </Button>
      </Box>

      <List>
        {skills.map((skill) => (
          <ListItem key={skill.id}>
            <ListItemText primary={skill.title} />
            <ListItemSecondaryAction>
              <IconButton edge="end" onClick={() => handleOpen(skill)}>
                <EditIcon />
              </IconButton>
              <IconButton edge="end" onClick={() => handleDelete(skill.id)}>
                <DeleteIcon />
              </IconButton>
            </ListItemSecondaryAction>
          </ListItem>
        ))}
      </List>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>
          {editingSkill ? 'Редактировать скилл' : 'Добавить скилл'}
        </DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Название скилла"
            fullWidth
            value={skillTitle}
            onChange={(e) => setSkillTitle(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Отмена</Button>
          <Button onClick={handleSubmit} variant="contained">
            {editingSkill ? 'Сохранить' : 'Добавить'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}; 