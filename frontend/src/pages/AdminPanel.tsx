import { useState } from 'react';
import { Box, Tabs, Tab, Typography } from '@mui/material';
import { SkillsManagement } from '../components/admin/SkillsManagement';
import { CategoriesManagement } from '../components/admin/CategoriesManagement';

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`admin-tabpanel-${index}`}
      aria-labelledby={`admin-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}

export const AdminPanel = () => {
  const [value, setValue] = useState(0);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  return (
    <Box sx={{ width: '100%', maxWidth: 1200, mx: 'auto', mt: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        Панель администратора
      </Typography>
      
      <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Tabs value={value} onChange={handleChange}>
          <Tab label="Скиллы" />
          <Tab label="Категории" />
        </Tabs>
      </Box>

      <TabPanel value={value} index={0}>
        <SkillsManagement />
      </TabPanel>
      <TabPanel value={value} index={1}>
        <CategoriesManagement />
      </TabPanel>
    </Box>
  );
}; 